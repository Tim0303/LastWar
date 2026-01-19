import axios from 'axios';

/**
 * 取得 JWT token
 * @returns
 */
function getJwtToken(): string | null {
  // 取得 user 的 accessToken
  const user = localStorage.getItem('user');
  if (user) {
    const parsedUser = JSON.parse(user);
    return parsedUser.accessToken || null;
  }
  return null;
}

/**
 * 儲存新 token
 * @param token
 */
function setJwtToken(token: string) {
  localStorage.setItem('user', token);
}

/**
 * 取得 refresh token
 * @returns
 */
function getRefreshToken(): string | null {
  // 取得 user
  const user = localStorage.getItem('user');
  if (user) {
    const parsedUser = JSON.parse(user);
    return parsedUser.refreshToken || null;
  }
  return null;
}

/**
 * 刷新 token 的 API
 * @returns
 */
async function refreshToken() {
  const refresh_token = getRefreshToken();
  if (!refresh_token) throw new Error('No refresh token');
  const response = await axios.post('/api/auth/refresh', { refresh_token });
  setJwtToken(response.data);
  if (response.data.refresh_token) {
    localStorage.setItem('user:refreshToken', response.data.refresh_token);
  }
  return response.data.access_token;
}

/**
 * 建立 axios 實例
 */
const api = axios.create({
  baseURL: '/api'
});

/**
 * 請求攔截器，自動帶入 JWT token
 */
api.interceptors.request.use((config) => {
  const token = getJwtToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

/**
 * 響應攔截器，自動處理 401 錯誤並刷新 token
 */
let isRefreshing = false;
let failedQueue: any[] = [];

function processQueue(error: any, token: string | null = null) {
  failedQueue.forEach((prom) => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  failedQueue = [];
}

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise(function (resolve, reject) {
          failedQueue.push({ resolve, reject });
        })
          .then((token) => {
            originalRequest.headers.Authorization = 'Bearer ' + token;
            return api(originalRequest);
          })
          .catch((err) => Promise.reject(err));
      }
      originalRequest._retry = true;
      isRefreshing = true;
      try {
        const newToken = await refreshToken();
        processQueue(null, newToken);
        originalRequest.headers.Authorization = 'Bearer ' + newToken;
        return api(originalRequest);
      } catch (err) {
        processQueue(err, null);
        // 可選：登出或導向登入頁
        return Promise.reject(err);
      } finally {
        isRefreshing = false;
      }
    }
    return Promise.reject(error);
  }
);

export default api;
