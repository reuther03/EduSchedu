<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()
</script>

<template>
  <div v-if="authStore.isLoggedIn">
    <RouterLink to="/school_create">School</RouterLink>
  </div>
</template>

<style scoped></style>
