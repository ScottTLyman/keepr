<template>
  <CreateModal id="create-keep">
    <template #modal-body>
      <form class="row d-flex" @submit.prevent="createKeep">
        <div class="col-12 d-flex flex-column mb-2 form-group">
          <button
            title="Close"
            type="button"
            class="btn-close align-self-end"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
          <label for="Keep Title" class="form-label fw-bold"
            ><h3>New Keep</h3></label
          >
          <input
            v-model="keep.name"
            required
            type="text"
            class="form-control mb-2"
            name="Keep Name"
            id=""
            aria-describedby="helpId"
            placeholder="Keep Name..."
          />
          <input
            v-model="keep.img"
            required
            type="text"
            class="form-control mb-2"
            name="Keep Image"
            id=""
            aria-describedby="helpId"
            placeholder="Image Url..."
          />
          <input
            v-model="keep.description"
            required
            type="textarea"
            class="form-control mb-2"
            name="Keep Description"
            id=""
            aria-describedby="helpId"
            placeholder="Keep Description..."
          />
          <button class="btn btn-success">Submit</button>
        </div>
      </form>
    </template>
  </CreateModal>
</template>



<script>
import { ref } from "@vue/reactivity"
import { useRouter } from "vue-router"
import { logger } from "../utils/Logger"
import Pop from "../utils/Pop"
import { Modal } from "bootstrap"
import { keepsService } from "../services/KeepsService"
export default {
  setup() {
    const router = useRouter()
    const keep = ref({})
    return {
      keep,
      async createKeep() {
        try {
          await keepsService.createKeep(keep.value)
          Modal.getOrCreateInstance(document.getElementById("create-vault")).hide();
          keep.value = {}
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