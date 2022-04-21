<template>
  <CreateModal id="create-vault">
    <template #modal-body>
      <form class="row d-flex" @submit.prevent="createVault">
        <div class="col-12 d-flex flex-column mb-2 form-group">
          <button
            title="Close"
            type="button"
            class="btn-close align-self-end"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
          <label for="Vault Title" class="form-label fw-bold"
            ><h3>New Vault</h3></label
          >
          <input
            v-model="vault.name"
            required
            type="text"
            class="form-control mb-2"
            name="Vault Name"
            id=""
            aria-describedby="helpId"
            placeholder="Vault Name..."
          />
          <input
            v-model="vault.img"
            required
            type="text"
            class="form-control mb-2"
            name="Vault Image"
            id=""
            aria-describedby="helpId"
            placeholder="Image Url..."
          />
          <div class="d-flex justify-content-between">
            <div class="d-flex flex-column">
              <input
                type="checkbox"
                name="Private"
                id=""
                v-model="vault.isPrivate"
              />
              <label for="Private"> Private?</label>
              <small class="text-muted"
                >Private vaults are only seen by you</small
              >
            </div>
            <button class="btn btn-success">Submit</button>
          </div>
        </div>
      </form>
    </template>
  </CreateModal>
</template>


<script>
import { ref } from "@vue/reactivity"
import { vaultsService } from "../services/VaultsService"
import { logger } from "../utils/Logger"
import Pop from "../utils/Pop"
import { Modal } from "bootstrap"
import { useRouter } from "vue-router"
import { profilesService } from "../services/ProfilesService"
import { AppState } from "../AppState"
export default {
  setup() {
    const router = useRouter()
    const vault = ref({})
    return {
      vault,
      async createVault() {
        try {
          let newVault = await vaultsService.createVault(vault.value)
          await profilesService.getProfileVaults(AppState.account.id)
          Modal.getOrCreateInstance(document.getElementById("create-vault")).hide();
          vault.value = {}
          // router.push({ name: "Vault", params: { id: newVault.id } })
        } catch (error) {
          logger.error(error)
          Pop.toast(error.message, 'error')
        }
      }
    }
  }
}
</script>


<style lang="scss" scoped>
</style>