<script setup lang="ts">

import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()
</script>

<template>
  <nav>
    <RouterLink class="navbar_item" to="/">EduSchedu</RouterLink>
    <div class="navbar_item">
      <div class="navbar_item" v-if="authStore.isLoggedIn">
        <RouterLink class="navbar_subitem mgr-10" to="/profile">{{ authStore.user?.fullName }}</RouterLink>
        <span class="navbar_subitem" @click="authStore.logout()">Logout</span>
      </div>
      <RouterLink v-else class="navbar_item" to="/login">Login</RouterLink>
    </div>
  </nav>
</template>

<style src="src/assets/styles/Navbar.css">
</style>
