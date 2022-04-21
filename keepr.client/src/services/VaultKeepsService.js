import { AppState } from "../AppState"
import { logger } from "../utils/Logger"
import { api } from "./AxiosService"

class VaultKeepsService {
  async getById(id) {
    const res = await api.get(`api/vaultKeeps/${id}`)
    logger.log(res.data, 'got VaultKeep by id')
    AppState.activeVKeep = res.data
  }
  async createVaultKeep(vkData) {
    const res = await api.post('api/vaultKeeps', vkData)
    logger.log(res.data, 'vk created')
    AppState.vaultKeeps.push(res.data)
    return res.data
  }
  async deleteVaultKeep(id) {
    const res = await api.delete(`api/vaultKeeps/${id}`)
    logger.log('vk deleted', res.data)
    AppState.vaultKeeps.filter(vk => vk.id != id)
  }
}

export const vaultKeepsService = new VaultKeepsService()