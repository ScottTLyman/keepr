<template>
  <div class="container-fluid bg-secondary screen-height">
    <div class="row">
      <div class="col-10 d-flex align-items-center p-5 text-shrink">
        <img
          class="img-fluid rounded"
          height="300"
          width="300"
          :src="profile.picture"
          alt=""
        />
        <div class="ps-3" v-if="user.id != profile.id">
          <h1 class="">{{ profile.name }}</h1>
          <h4>Vaults: {{ vCount }}</h4>
          <h4>Keeps: {{ kCount }}</h4>
        </div>
        <div class="ps-3" v-else>
          <h1 class="">{{ profile.name }}</h1>
          <h4>Vaults: {{ mCount }}</h4>
          <h4>Keeps: {{ kCount }}</h4>
        </div>
      </div>
    </div>
    <div class="row">
      <h3 class="px-5 t-shadow fw-bold">
        Vaults<i
          v-if="profile.id == user.id"
          class="mdi mdi-plus selectable"
          title="Create new Vault"
          data-bs-toggle="modal"
          data-bs-target="#create-vault"
        ></i>
      </h3>
    </div>
    <div class="masonry-with-columns" v-if="profile.id != user.id">
      <div class="p-2" v-for="v in pVaults" :key="v.id">
        <div class="card bg-secondary text-white selectable elevation-2">
          <img class="img-fluid rounded" :src="v.img" alt="" />
          <div
            class="
              card-img-overlay
              d-flex
              align-items-end
              justify-content-between
              p-0
              px-1
              pb-1
              img-gradient
            "
            @click="goToVault(v.id)"
          >
            <h5 class="card-title t-shadow">{{ v.name }}</h5>
          </div>
        </div>
      </div>
    </div>
    <div class="masonry-with-columns" v-else>
      <div class="p-2" v-for="v in mVaults" :key="v.id">
        <div class="card bg-secondary text-white selectable elevation-2">
          <img class="img-fluid rounded" :src="v.img" alt="" />
          <div
            class="
              card-img-overlay
              d-flex
              align-items-end
              justify-content-between
              p-0
              px-1
              pb-1
              img-gradient
            "
            @click="goToVault(v.id)"
          >
            <h5 class="card-title t-shadow">{{ v.name }}</h5>

            <!-- <small class="text-warning"> EMPTY</small> -->
            <i
              class="mdi mdi-lock-outline text-warning"
              v-show="v.isPrivate == true"
              title="Private Vault"
            ></i>
          </div>
        </div>
      </div>
    </div>
    <div class="row mt-2">
      <h3 class="px-5 fw-bold t-shadow">
        Keeps<i
          data-bs-toggle="modal"
          data-bs-target="#create-keep"
          v-if="profile.id == user.id"
          class="mdi mdi-plus t-shadow selectable"
          title="Create new Keep"
        ></i>
      </h3>
    </div>
    <div class="masonry-with-columns">
      <div class="py-2" v-for="k in pKeeps" :key="k.id">
        <div class="card bg-secondary text-white elevation-2 selectable">
          <img class="img-fluid rounded" :src="k.img" alt="" />
          <div
            data-bs-toggle="modal"
            data-bs-target="#active-keep"
            @click="setActive(k.id)"
            class="
              card-img-overlay
              d-flex
              align-items-end
              justify-content-between
              p-0
              px-1
              pb-1
              img-gradient
            "
          >
            <h5 class="card-title t-shadow">{{ k.name }}</h5>
          </div>
        </div>
      </div>
    </div>
  </div>
  <CreateVaultModal />
  <CreateKeepModal />
  <KeepDetailsModal />
</template>

<script>
import { computed, onMounted, watchEffect } from "@vue/runtime-core"
import { useRoute, useRouter } from "vue-router"
import { logger } from "../utils/Logger"
import { profilesService } from "../services/ProfilesService"
import Pop from "../utils/Pop"
import { AppState } from "../AppState"
import { accountService } from "../services/AccountService"
import { router } from "../router"
import { keepsService } from "../services/KeepsService"
import { vaultKeepsService } from "../services/VaultKeepsService"
import { vaultsService } from "../services/VaultsService"
export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    onMounted(async () => {
      try {
        if (route) {
          await profilesService.getProfile(route.params.id)
        }
      } catch (error) {
        logger.error(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      user: computed(() => AppState.account),
      profile: computed(() => AppState.profile),
      pVaults: computed(() => AppState.profileVaults),
      pKeeps: computed(() => AppState.profileKeeps),
      mVaults: computed(() => AppState.myVaults),
      mCount: computed(() => AppState.myVaults.length),
      kCount: computed(() => AppState.profileKeeps.length),
      vCount: computed(() => AppState.profileVaults.length),
      vKeeps: computed(() => AppState.vaultKeeps),
      goToVault(id) {
        router.push({ name: 'Vault', params: { id } })
      },
      async setActive(id) {
        try {
          await keepsService.getById(id)
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
    }
  }
}
</script>

<style scoped lang="scss">
.masonry-with-columns {
  columns: 6 200px;
  column-gap: 0.5rem;
  div {
    display: inline-flex;
    width: 100%;
  }
}
.grid {
  display: grid;
  height: 100px;
  grid-template-rows: 30px 1fr;
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
.screen-height {
  height: 100%;
}
.text-shrink {
  overflow: hidden;
}
</style>
