<template>
  <div class="container-fluid bg-secondary screen-height">
    <div class="row">
      <div class="col-10 d-flex align-items-center justify-content-between p-5">
        <div class="ps-3">
          <h1>{{ vault.name }}</h1>
          <h4>Keeps: {{ kCount }}</h4>
        </div>
        <div>
          <button
            v-if="vault.creator?.id == account?.id"
            class="btn btn-outline-danger"
            @click="deleteVault"
          >
            Delete Vault
          </button>
        </div>
      </div>
    </div>
    <div class="masonry-with-columns">
      <div v-for="k in vKeeps" :key="k.id">
        <div class="card bg-secondary text-white elevation-2 mb-2">
          <img class="img-fluid rounded" :src="k?.img" alt="" />
          <div
            class="
              card-img-overlay
              d-flex
              justify-content-between
              p-0
              px-1
              pb-1
              img-gradient
            "
          >
            <i
              v-if="vault.creator?.id == account.id"
              title="Remove Keep from Vault"
              @click="deleteVaultKeep(k.vaultKeepId)"
              class="mdi mdi-trash-can-outline text-danger selectable"
            ></i>
            <div class="d-flex align-self-end justify-content-between">
              <h5
                title="See Details"
                class="card-title t-shadow selectable"
                @click="setActive(k.id)"
                data-bs-toggle="modal"
                data-bs-target="#active-vKeep"
              >
                {{ k.name }}
              </h5>
              <img
                title="Go To Profile"
                @click="goToProfile(k.creator?.id)"
                class="rounded-circle selectable"
                :src="k.creator?.picture"
                height="40"
                width="40"
                alt=""
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <VaultKeepDetailsModal />
</template>


<script>
import { vaultsService } from "../services/VaultsService"
import { keepsService } from "../services/KeepsService"
import { computed, onMounted, watchEffect } from "@vue/runtime-core"
import { AppState } from "../AppState"
import { useRoute, useRouter } from "vue-router"
import { logger } from "../utils/Logger"
import Pop from "../utils/Pop"
import { vaultKeepsService } from "../services/VaultKeepsService"
import { Modal } from "bootstrap"
export default {
  setup() {
    const route = useRoute()
    const router = useRouter()
    onMounted(async () => {
      try {
        await vaultsService.getById(route.params.id)
        await vaultsService.getVaultKeeps(route.params.id)
      } catch (error) {
        router.push({ name: 'Home' })
        logger.error(error)
        // Pop.toast(error.message, 'error')
      }
    })
    return {
      account: computed(() => AppState.account),
      vKeeps: computed(() => AppState.vaultKeeps),
      vault: computed(() => AppState.activeVault),
      vKeep: computed(() => AppState.activeVKeep),
      kCount: computed(() => AppState.vaultKeeps.length),
      async deleteVault() {
        try {
          if (await Pop.confirm("Delete your vault?")) {
            await vaultsService.deleteVault(route.params.id)
            router.push({ name: 'Profile', params: { id: this.vault.creatorId } })
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      async setActive(id) {
        try {
          await keepsService.getById(id)
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      async deleteVaultKeep(id) {
        try {
          if (await Pop.confirm("Remove keep from Vault?")) {
            await vaultKeepsService.deleteVaultKeep(id)
            await vaultsService.getVaultKeeps(AppState.activeVault.id)
          }
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      goToProfile(id) {
        router.push({ name: 'Profile', params: { id } })
        Modal.getOrCreateInstance(document.getElementById("active-keep")).hide();
      },
    }
  }
}
</script>


<style lang="scss" scoped>
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
.screen-height {
  height: 100vh;
}
</style>