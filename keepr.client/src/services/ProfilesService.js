import { AppState } from "../AppState"
import { logger } from "../utils/Logger"
import { api } from "./AxiosService"

class ProfilesService {
  async getProfile(id) {
    const res = await api.get(`api/profiles/${id}`)
    logger.log(res.data, 'got profile')
    AppState.profile = res.data
  }
  async getProfileVaults(id) {
    const res = await api.get(`api/profiles/${id}/vaults`)
    logger.log(res.data, 'got profile vaults')
    AppState.profileVaults = res.data
  }
  async getProfileKeeps(id) {
    const res = await api.get(`api/profiles/${id}/keeps`)
    logger.log(res.data, 'got profile keeps')
    AppState.profileKeeps = res.data
  }
}

export const profilesService = new ProfilesService()