import api from '@/utils/axios';
import { Authenticate } from '@/api/constants';

export function login(data: { account: string; secret: string; captcha?: string }) {
  return api.post(Authenticate.LOGIN, data);
}

export function refreshToken(refresh_token: string) {
  return api.post(Authenticate.REFRESHTOKEN, { refresh_token });
}
