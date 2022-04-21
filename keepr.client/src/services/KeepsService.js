import { AppState } from "../AppState"
import { logger } from "../utils/Logger"
import { api } from "./AxiosService"

class KeepsService {
  async getAll() {
    const res = await api.get('api/keeps')
    logger.log(res.data)
    AppState.keeps = res.data
  }
  async getById(id) {
    const res = await api.get(`api/keeps/${id}`)
    logger.log(res.data, 'got keep by id')
    AppState.activeKeep = res.data
  }
  async createKeep(keepData) {
    const res = await api.post('api/keeps', keepData)
    logger.log('keep data', res.data)
    AppState.keeps.push(res.data)
    return res.data
  }
  async deleteKeep(id) {
    const res = await api.delete(`api/keeps/${id}`)
    logger.log('keep deleted', res.data)
    AppState.keeps.filter(v => v.id != id)
    return res.data
  }
}
export const keepsService = new KeepsService()