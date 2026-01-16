import api from '@/utils/axios';
import { WeatherForecast } from '@/api/constants';

export function getWeatherForecast() {
  return api.get(WeatherForecast.GET);
}
