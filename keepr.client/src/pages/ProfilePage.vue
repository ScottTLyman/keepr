<template>
  <div class="container-fluid bg-secondary">
    <div class="row">
      <div class="col-10 d-flex align-items-center p-5">
        <img
          class="img-fluid"
          height="300"
          width="300"
          :src="profile.picture"
          alt=""
        />
        <div class="ps-3">
          <h1>{{ profile.name }}</h1>
          <h4>Vaults: {{ vaultsCount }}</h4>
          <h4>Keeps: {{ keepsCount }}</h4>
        </div>
      </div>
    </div>
    <div class="row">
      <h3 class="px-5">
        Vaults<i class="mdi mdi-plus text-success t-shadow selectable"></i>
      </h3>
    </div>
    {{ profileKeeps }}
  </div>
</template>

<script>
import { computed, onMounted } from "@vue/runtime-core"
import { useRoute } from "vue-router"
import { logger } from "../utils/Logger"
import { profilesService } from "../services/ProfilesService"
import Pop from "../utils/Pop"
import { AppState } from "../AppState"
export default {
  setup() {
    const route = useRoute()
    onMounted(async () => {
      try {
        await profilesService.getProfile(route.params.id)
        await profilesService.getProfileVaults(route.params.id)
        await profilesService.getProfileKeeps(route.params.id)
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      profile: computed(() => AppState.profile),
      profileVaults: computed(() => AppState.profileVaults),
      profileKeeps: computed(() => AppState.profileKeeps),
      keepsCount: computed(() => AppState.profileKeeps.length),
      vaultsCount: computed(() => AppState.profileVaults.length)
    }
  }
}
</script>

<style scoped lang="scss">
.masonry-with-columns {
  columns: 6 200px;
  column-gap: 1rem;
  div {
    display: inline-block;
    width: 100%;
  }
}
.img-gradient {
  background: linear-gradient(
    to bottom,
    rgba(185, 184, 184, 0),
    rgba(48, 47, 48, 0.8)
  );
}
.t-shadow {
  text-shadow: 2px 2px 2px #00d9ff;
}
</style>
