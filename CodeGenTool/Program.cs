using NETWebTemp.Common.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

internal class Program
{
    /// <summary>
    /// 方案名稱
    /// </summary>
    private static readonly string rootNamespace = "NETWebTemp";

    private static Dictionary<string, string>? referencedTypes;

    /// <summary>
    /// 輸出路徑
    /// </summary>
    private static string _outPutPath = string.Empty;

    /// <summary>
    /// 要忽略的類型名稱(選填)
    /// </summary>
    private static List<string> _ignoreTypes = new();

    /// <summary>
    /// 前端專案資料夾名稱
    /// </summary>
    private static readonly string _frontendRoot = "ia.lgofms.client";

    /// <summary>
    /// 轉換資料夾
    /// </summary>
    private static readonly List<string> _targetFolders = new List<string> { "ViewModels", "Parameters", "Enums", "Interfaces" };

    /// <summary>
    /// 輸出資料夾
    /// </summary>
    private static readonly List<string> _outputFolders = new List<string> { "ViewModels", "Parameters", "Enums" };

    private static void Main(string[] args)
    {
        // 取得應用程式的根目錄
        string baseDirectory = AppContext.BaseDirectory;

        // 組合相對路徑
        string frontendRoot = Path.Combine(baseDirectory, @$"..\..\..\..\{_frontendRoot}\types");

        // 轉換相對路徑成為絕對路徑
        _outPutPath = Path.GetFullPath(frontendRoot);

        // 輸出目錄
        Console.WriteLine("輸出到: " + _outPutPath);

        // 設定要忽略的類型名稱 忽略class和interface檔案
        _ignoreTypes = new List<string>
        {
            "ApiResponse"
        };

        // 確保資料夾存在，如果不存在則創建
        if (!Directory.Exists(_outPutPath))
        {
            Directory.CreateDirectory(_outPutPath);
        }

        // 載入程序集
        var assemblys = new List<Assembly>()
        {
            Assembly.Load($"{rootNamespace}.Service"),
            Assembly.Load($"{rootNamespace}.Common")
        };

        foreach (var assembly in assemblys)
        {
            var xml = LoadXmlComments(assembly);

            var namespaces = assembly.GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.StartsWith(rootNamespace) /*&& t.Attributes.GetAttributes().Any(a=> a.)*/)
                .Select(t => t.Namespace!)
                .Distinct()
                .Where(ns => _targetFolders.Any(folder => ns.Split('.').Contains(folder)))
                .ToList();

            foreach (var ns in namespaces)
            {
                GenerateTypeScriptInterfacesForModels(assembly, xml, ns);
            }
        }
    }

    /// <summary>
    /// 生成 TypeScript 介面
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="xml"></param>
    /// <param name="targetNamespace"></param>
    private static void GenerateTypeScriptInterfacesForModels(Assembly assembly, XDocument? xml, string targetNamespace)
    {
        // 取得資料夾名稱（namespace 最後一段）
        var folderName = targetNamespace.Split('.').FirstOrDefault(n => _outputFolders.Any(a => n == a)) ?? "Others";
        var subFolderName = targetNamespace.Split('.').Last(); // 資料夾名稱
        var tsFileName = subFolderName;

        // 取得所有 class/interface
        var types = assembly.GetTypes()
            .Where(t => (t.IsClass || t.IsInterface) &&
            t.Namespace == targetNamespace &&
            t.GetCustomAttribute<CodeGenIgnoreAttribute>() == null)
            .ToList();

        // 取得所有 enum
        var enumTypes = assembly.GetTypes()
            .Where(t => t.IsEnum &&
            t.Namespace == targetNamespace &&
            t.GetCustomAttribute<CodeGenIgnoreAttribute>() == null)
            .ToList();

        // 收集 import、interface、enum 內容
        referencedTypes = new Dictionary<string, string>();
        var importDic = new Dictionary<string, List<string>>();
        var importLines = new StringBuilder();
        var contentLines = new StringBuilder();

        foreach (var type in types)
        {
            if (_ignoreTypes.Any(a => type.Name == a || type.Name.Contains(a + "`")))
            {
                continue;
            }
            var tsContent = GenerateTypeScriptInterface(type, xml);
            contentLines.AppendLine(tsContent);
        }

        foreach (var type in enumTypes)
        {
            if (_ignoreTypes.Any(a => type.Name == a || type.Name.Contains(a + "`")))
            {
                continue;
            }
            var tsEnum = ConvertEnumToTypeScript(type);
            contentLines.AppendLine(tsEnum);

            if (enumTypes.IndexOf(type) + 1 != enumTypes.Count)
            {
                contentLines.AppendLine();
            }
        }

        // 產生 import 語句
        foreach (var kv in referencedTypes)
        {
            var typeName = kv.Key;
            var fullNamespace = kv.Value;

            var refFolder = fullNamespace.Split('.').FirstOrDefault(n => _outputFolders.Any(a => n == a)) ?? "Others";
            var refSubFolder = fullNamespace.Split('.').Last();

            if (refFolder == folderName && refSubFolder == subFolderName)
                continue;

            // 決定匯入路徑
            string importPath;
            if (refFolder == refSubFolder)
                importPath = $"../{ToLowerFirstChar(refFolder)}";
            else
                importPath = $"../{ToLowerFirstChar(refFolder)}/{ToLowerFirstChar(refSubFolder)}";

            // 將 typeName 加入對應匯入路徑的清單
            if (!importDic.ContainsKey(importPath))
                importDic[importPath] = new List<string>();

            if (!importDic[importPath].Contains(typeName))
                importDic[importPath].Add(typeName);
        }

        // 將匯入路徑合併後產出 import 語句
        foreach (var kv in importDic.OrderBy(x => x.Key))
        {
            var importPath = kv.Key;
            var typeNames = kv.Value.OrderBy(n => n); // 排序 type 名稱以維持一致性
            importLines.AppendLine($"import type {{ {string.Join(", ", typeNames)} }} from '{importPath}'");
        }
        if (importLines.Length > 0) importLines.AppendLine();

        // 合併 import 和內容
        var tsFileContent = importLines.ToString() + contentLines.ToString();

        // 確保結尾只有一個換行
        tsFileContent = tsFileContent.TrimEnd('\r', '\n') + Environment.NewLine;

        // 儲存到對應資料夾
        SaveToFile(folderName, tsFileName, tsFileContent);
    }

    private static string GenerateTypeScriptInterface(Type type, XDocument? xml)
    {
        var sb = new StringBuilder();

        var summary = GetXmlSummary(xml, type);
        if (!string.IsNullOrEmpty(summary))
        {
            sb.AppendLine($"/** {summary} */");
        }

        // 取得 baseType 和 interfaces
        Type? baseType = type.BaseType;
        var interfaceNames = type.GetInterfaces().Select(i => GetTypeName(i)).ToList();

        // 處理 extends 部分
        var inheritanceList = new List<string>();
        if (baseType != null && baseType.Name != "Object")
        {
            inheritanceList.Add(GetTypeName(baseType));
        }

        // 處理從其他命名空間來的 class
        if (baseType != null &&
            type.Namespace != baseType.Namespace &&
            baseType.Namespace != "System" &&
            baseType != typeof(object))
        {
            referencedTypes[GetTypeName(baseType)] = baseType.Namespace;
        }

        foreach (var interfaceType in type.GetInterfaces())
        {
            if (type.Namespace != interfaceType.Namespace)
            {
                referencedTypes[GetTypeName(interfaceType)] = interfaceType.Namespace;
            }
        }

        var extendsClause = inheritanceList.Count > 0
            ? $" extends {string.Join(", ", inheritanceList, interfaceNames)}"
            : "";

        var allParents = inheritanceList.Concat(interfaceNames).ToList();
        var allExtendsClause = allParents.Count > 0
            ? $" extends {string.Join(", ", allParents)}"
            : "";

        sb.AppendLine($"export interface {GetTypeName(type)}{allExtendsClause} {{");

        // 所有 interface 宣告的屬性
        var interfaceProps = type.GetInterfaces()
            .SelectMany(i => i.GetProperties())
            .Select(p => p.Name)
            .ToHashSet(); // 只比對名稱（必要時可連型別比對）

        // 從所有 class 宣告的 public instance 屬性（注意：這裡包含 interface 要求的欄位）排除 interface 欄位
        List<PropertyInfo> classProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(p => !interfaceProps.Contains(p.Name) && p.GetCustomAttribute<CodeGenIgnoreAttribute>() == null) // 排除介面欄位
            .ToList();

        foreach (var prop in classProps)
        {
            var propSummary = GetXmlSummary(xml, prop);
            // 判斷該屬性是否為必填 (使用 Required 特性)
            var isRequired = prop.GetCustomAttributes(typeof(RequiredAttribute), false).Any();

            if (!string.IsNullOrEmpty(propSummary))
            {
                string description = isRequired
                    ? $"{propSummary}－必填"
                    : propSummary;

                // 檢查 MaxLength 特性
                var maxLengthAttr = prop.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault() as MaxLengthAttribute;
                description = maxLengthAttr != null
                    ? $"{description}－最大長度{maxLengthAttr.Length}"
                    : description;

                // 檢查 MinLength 特性
                var minLengthAttr = prop.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault() as MinLengthAttribute;
                description = minLengthAttr != null
                    ? $"{description}－最小長度{minLengthAttr.Length}"
                    : description;

                // 檢查 Length 特性
                var lengthAttr = prop.GetCustomAttributes(typeof(LengthAttribute), false).FirstOrDefault() as LengthAttribute;
                description = lengthAttr != null
                    ? $"{description}－長度範圍{lengthAttr.MinimumLength}至{lengthAttr.MaximumLength}"
                    : description;

                sb.AppendLine($"  /** {description} */");
            }

            var tsType = MapCSharpTypeToTs(prop.PropertyType, type.Namespace);
            var isNullable = IsReferenceTypeNullable(prop);

            // TypeScript 型別：若非必填則加上 "| null"
            string tsTypeWithNull = isNullable ? $"{tsType} | null" : tsType;

            // TypeScript 欄位格式：若非必填則加上 "?"
            var tsField = $"{ToLowerFirstChar(prop.Name)}{(isRequired ? "" : "?")}: {tsTypeWithNull}";

            sb.AppendLine($"  {tsField}");
        }

        sb.AppendLine("}");

        string result = sb.ToString();

        return result;
    }

    /// <summary>
    /// 載入 XML 註解文件
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    private static XDocument? LoadXmlComments(Assembly assembly)
    {
        var xmlPath = Path.ChangeExtension(assembly.Location, ".xml");
        if (File.Exists(xmlPath))
        {
            return XDocument.Load(xmlPath);
        }

        Console.WriteLine("// ⚠️ 找不到 XML 文件：" + xmlPath);
        return null;
    }

    /// <summary>
    /// 儲存到指定資料夾的檔案
    /// </summary>
    /// <param name="folderName"></param>
    /// <param name="fileName"></param>
    /// <param name="tsContent"></param>
    private static void SaveToFile(string folderName, string fileName, string tsContent)
    {
        // 加入 eslintignore
        tsContent = $"/* eslint-disable */\n{tsContent}";

        folderName = ToLowerFirstChar(folderName);
        fileName = ToLowerFirstChar(fileName);

        string dirPath = Path.Combine(_outPutPath, folderName);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        // 資料夾跟檔名相同，則檔名改為 index
        fileName = folderName == fileName ? "index" : fileName;

        string filePath = Path.Combine(dirPath, $"{fileName}.ts");
        File.WriteAllText(filePath, tsContent, Encoding.UTF8);
        Console.WriteLine(tsContent);
        Console.WriteLine($"已儲存檔案：{filePath}");
    }

    private static string GetXmlSummary(XDocument? xml, MemberInfo member)
    {
        if (xml == null) return "";

        string memberName = member switch
        {
            Type t => $"T:{t.FullName}",
            PropertyInfo p => $"P:{p.DeclaringType?.FullName}.{p.Name}",
            _ => ""
        };

        var summary = xml.Descendants("member")
            .Where(x => x.Attribute("name")?.Value == memberName)
            .Select(x => x.Element("summary")?.Value.Trim())
            .FirstOrDefault();

        return summary ?? "";
    }

    private static string MapCSharpTypeToTs(Type type, string nameSpace)
    {
        // 檢查 Nullable 類型，去掉 Nullable 參數
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            var underlyingType = type.GetGenericArguments()[0];

            return MapCSharpTypeToTs(underlyingType, nameSpace);
        }

        // 檢查是否為 List 類型
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            // 處理 List 中的泛型類型
            var elementType = type.GenericTypeArguments[0];

            return $"{MapCSharpTypeToTs(elementType, nameSpace)}[]";
        }

        // Dictionary<TKey, TValue>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            var keyType = type.GenericTypeArguments[0];
            var valueType = type.GenericTypeArguments[1];

            var tsKeyType = MapBasicTypeToTs(keyType, nameSpace); // key 通常是 string 或 number
            var tsValueType = MapCSharpTypeToTs(valueType, nameSpace); // value 可能是巢狀 Dictionary

            return $"{{ [key: {tsKeyType}]: {tsValueType} }}";
        }

        return MapBasicTypeToTs(type, nameSpace);
    }

    /// <summary>
    /// 處理基本類型與命名空間檢查
    /// </summary>
    /// <param name="type"></param>
    /// <param name="nameSpace"></param>
    /// <returns></returns>
    private static string MapBasicTypeToTs(Type type, string nameSpace)
    {
        // 檢查基本類型
        if (type == typeof(string) || type == typeof(Guid))
            return "string";
        if (type == typeof(int) || type == typeof(double) || type == typeof(decimal))
            return "number";
        if (type == typeof(bool))
            return "boolean";
        if (type.Name == "DateOnly" || type.Name == "DateTime")
            return "Date";

        // 處理來自其他命名空間的類型
        if (type.Namespace != null && type.Namespace.StartsWith(rootNamespace))
        {
            if (type.Namespace != nameSpace)
            {
                referencedTypes[GetTypeName(type)] = type.Namespace;
            }

            // 忽略掉名稱中的符號（忽略泛型）
            return GetTypeName(type);
        }

        // 其他情況，返回 'any' 類型
        return "any";
    }

    /// <summary>
    /// 值型別可以簡單判斷 nullable，reference 型別則需要檢查 NullableAttribute
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    public static bool IsReferenceTypeNullable(PropertyInfo propertyInfo)
    {
        var isNullable = false;

        // 如果有 RequiredAttribute，則不允許為 null
        //if (propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false).Any())
        //{
        //    isNullable = false;
        //    return isNullable;
        //}

        var nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = nullabilityContext.Create(propertyInfo);
        isNullable = nullabilityInfo.WriteState == NullabilityState.Nullable;

        return isNullable;
    }

    private static string GetTypeName(Type type)
    {
        string cleanTypeName = type.Name;

        // 處理泛型，將泛型參數部分以 <T> 替代
        if (type.IsGenericType)
        {
            // 處理泛型類型的參數名稱（例如 Dictionary<int, string> -> Dictionary<T, T>）
            var genericArguments = type.GetGenericArguments();
            var genericTypeNames = string.Join(",", genericArguments.Select(t => MapBasicTypeToTs(t, t.Namespace)));

            cleanTypeName = System.Text.RegularExpressions.Regex.Replace(type.Name, "`\\d+", $"<{genericTypeNames}>");
        }
        return cleanTypeName;
    }

    /// <summary>
    /// Enum 轉換
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static string ConvertEnumToTypeScript(Type enumType)
    {
        if (enumType.IsEnum)
        {
            var enumName = enumType.Name;
            // 取得每個枚舉成員的名稱和值
            var enumValues = Enum.GetValues(enumType)
                                 .Cast<Enum>()
                                 .Select(e =>
                                 {
                                     var description = GetEnumDescription(e);
                                     return $"  /** {description} */\n  {ToLowerFirstChar(Enum.GetName(enumType, e))} = {Convert.ToInt32(e)}";
                                 });

            // 生成 TypeScript enum
            var tsEnum = $"export enum {enumName} {{\n{string.Join(",\n", enumValues)}\n}}";
            return tsEnum;
        }

        return string.Empty;
    }

    /// <summary>
    /// 取得枚舉描述
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute?.Description ?? value.ToString(); // 如果沒有描述則返回枚舉成員名稱
    }

    /// <summary>
    /// 把字串開頭轉化為小寫
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToLowerFirstChar(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentNullException(nameof(str));

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }
}