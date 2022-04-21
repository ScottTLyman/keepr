<template>
  <Modal id="active-keep">
    <template #modal-body>
      <div class="container">
        <div class="row">
          <div class="col-md-6 p-2 rounded">
            <img
              class="w-100 object-fit-cover rounded"
              height="500"
              :src="keep.img"
              alt=""
            />
          </div>
          <div class="col-md-6 p-2 d-flex flex-column justify-content-between">
            <div class="d-flex flex-column">
              <button
                title="Close"
                type="button"
                class="btn-close align-self-end"
                data-bs-dismiss="modal"
                aria-label="Close"
              ></button>
              <div class="row p-2 justify-content-center">
                <div class="col-10 text-center">
                  <i
                    class="mdi mdi-glasses me-2 text-info fs-5"
                    title="Times Viewed"
                  >
                  </i
                  >{{ keep.views }} |
                  <i
                    class="mdi mdi-safe me-2 text-info fs-5"
                    title="Times added to Vault"
                  ></i
                  >{{ keep.kept }}
                  <h1 class="mt-2">{{ keep.name }}</h1>
                </div>
                <div class="row justify-content-center">
                  <div class="col-10">
                    <p>
                      {{ keep.description }}
                    </p>
                  </div>
                </div>
              </div>
            </div>
            <div class="row justify-content-center">
              <div class="col-10 border-bottom"></div>
            </div>
            <div class="row justify-content-center">
              <div
                class="col-10 d-flex justify-content-between align-items-center"
              >
                <select v-model="vaultId">
                  <option v-for="v in vaults" :key="v.id" :value="v.id">
                    {{ v.name }}
                  </option>
                </select>
                <button
                  title="Add to Vault"
                  class="btn btn-success"
                  @click="addVaultKeep"
                >
                  +
                </button>
                <i
                  title="Delete Keep"
                  @click="deleteKeep(keep.id)"
                  v-if="account.id == keep.creator?.id"
                  class="mdi mdi-trash-can-outline text-danger fs-5 selectable"
                ></i>
                <div
                  class="bg-secondary rounded selectable"
                  @click="goToProfile(keep.creator.id)"
                >
                  <img
                    alt="account photo"
                    :src="keep.creator?.picture"
                    height="40"
                    class="rounded-start"
                  />
                  <span class="mx-3 text-success lighten-30">{{
                    keep.creator?.name
                  }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>
  </Modal>
</template>


<script>
import { vaultKeepsService } from "../services/VaultKeepsService"
import { computed, ref } from "@vue/reactivity"
import { AppState } from "../AppState"
import { Modal } from "bootstrap"
import { logger } from "../utils/Logger"
import { onMounted, watchEffect } from "@vue/runtime-core"
import { accountService } from "../services/AccountService"
import { router } from "../router"
import { useRouter } from "vue-router"
import Pop from "../utils/Pop"
import { keepsService } from "../services/KeepsService"
export default {
  setup() {

    const router = useRouter()
    const vaultId = ref('')
    return {
      vaultId,
      user: computed(() => AppState.user),
      account: computed(() => AppState.account),
      keep: computed(() => AppState.activeKeep),
      vaults: computed(() => AppState.myVaults),
      async addVaultKeep() {
        try {

          const newVaultKeep = { keepId: AppState.activeKeep.id, vaultId: vaultId.value }
          logger.log('body', newVaultKeep)
          await vaultKeepsService.createVaultKeep(newVaultKeep)
          Modal.getOrCreateInstance(document.getElementById("active-keep")).hide();
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      },
      goToProfile(id) {
        router.push({ name: 'Profile', params: { id } })
        Modal.getOrCreateInstance(document.getElementById("active-keep")).hide();
      },
      async deleteKeep(id) {
        if (await Pop.confirm('Delete this Keep?')) {
          await keepsService.deleteKeep(id)
          Modal.getOrCreateInstance(document.getElementById('active-keep')).hide();
        }

      }
    }
  }
}
</script>


<style lang="scss" scoped>
.menu-scroll {
  overflow: scroll;
  height: 200px;
}
</style>