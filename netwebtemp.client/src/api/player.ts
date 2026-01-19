import api from '@/utils/axios';
import { Player } from '@/api/constants';

/**
 * 取得玩家戰力清單
 */
export const APIGETPLAYERLIST = (params: object) => {
  return api.get(Player.LIST, { params: params });
};

/**
 * 建立新玩家資訊
 * @param data
 * @returns
 */
export const APICREATEPLAYER = (data: unknown) => {
  return api.post(Player.CREATE, data);
};

/**
 * 更新玩家戰力
 */
export const APIUPDATEPLAYER = (data: unknown) => {
  return api.post(Player.UPDATE, data);
};

/**
 * 取得新玩家資訊
 */
export const APIGETDETAIL = (playerId: string) => {
  return api.get(Player.GETDETAIL, { params: { playerId } });
};
