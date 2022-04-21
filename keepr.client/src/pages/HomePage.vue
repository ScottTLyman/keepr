<template>
  <div class="container-fluid bg-secondary">
    <div class="masonry-with-columns">
      <div class="p-2" v-for="k in keeps" :key="k.id">
        <div
          class="card bg-secondary text-white selectable elevation-2"
          data-bs-toggle="modal"
          data-bs-target="#active-keep"
          @click="setActive(k.id)"
        >
          <img
            class="img-fluid rounded"
            :src="k.img"
            :alt="k.name + ' picture'"
          />
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
          >
            <h5 class="card-title t-shadow">{{ k.name }}</h5>
            <img
              class="rounded-circle"
              :src="k.creator.picture"
              height="40"
              width="40"
              alt=""
            />
          </div>
        </div>
      </div>
    </div>
  </div>
  <KeepDetailsModal />
</template>

<script>
import { logger } from "../utils/Logger"
import Pop from "../utils/Pop"
import { keepsService } from "../services/KeepsService"
import { computed } from "@vue/reactivity"
import { AppState } from "../AppState"
import { onMounted, watch, watchEffect } from "@vue/runtime-core"
import { useRoute, useRouter } from "vue-router"
import { accountService } from "../services/AccountService"
import { Modal } from "bootstrap"
export default {
  name: 'Home',
  setup() {
    const route = useRoute()
    const router = useRouter()
    watchEffect(async () => {
      try {
        await keepsService.getAll()
      } catch (error) {
        logger.log(error)
        Pop.toast(error.message, 'error')
      }
    })
    return {
      account: computed(() => AppState.account),
      user: computed(() => AppState.user),
      keeps: computed(() => AppState.keeps),
      async setActive(id) {
        try {
          await keepsService.getById(id)
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
