<template>
  <Modal id="active-vKeep">
    <template #modal-body>
      <div class="container">
        <div class="row">
          <div class="col-md-6 p-2 rounded">
            <img
              class="w-100 detail-img rounded"
              height="500"
              :src="keep?.img"
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
import { computed } from "@vue/reactivity"
import { AppState } from "../AppState"
import { logger } from "../utils/Logger"
import Pop from "../utils/Pop"
import { vaultKeepsService } from "../services/VaultKeepsService"
import { useRouter } from "vue-router"
import { Modal } from "bootstrap"
export default {
  // props: {
  //   vaultKeep: {
  //     type: Object,
  //     required: true
  //   }
  // },
  setup() {
    const router = useRouter()
    return {
      vaultKeeps: computed(() => AppState.vaultKeeps),
      keep: computed(() => AppState.activeKeep),
      goToProfile(id) {
        router.push({ name: 'Profile', params: { id } })
        Modal.getOrCreateInstance(document.getElementById("active-keep")).hide();
      },
    }
  }
}
</script>


<style lang="scss" scoped>
.detail-img {
  object-fit: cover;
}
</style>