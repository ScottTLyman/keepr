import { AppState } from "../AppState"
import { logger } from "../utils/Logger"
import { api } from "./AxiosService"

class VaultsService {
  async getById(id) {
    const res = await api.get(`api/vaults/${id}`)
    logger.log(res.data, 'got keep by id')
    AppState.activeVault = res.data
  }
  async getVaultKeeps(id) {
    const res = await api.get(`api/vaults/${id}/keeps`)
    logger.log(res.data, 'got vault keeps')
    AppState.vaultKeeps = res.data
  }
  async createVault(vaultData) {
    const res = await api.post('api/vaults', vaultData)
    logger.log('vault data', res.data)
    AppState.vaults.push(res.data)
    return res.data
  }
  async deleteVault(id) {
    const res = await api.delete(`api/vaults/${id}`)
    logger.log('vault deleted', res.data)
    AppState.vaults.filter(v => v.id != id)
    return res.data
  }
}

export const vaultsService = new VaultsService()