<script setup lang="ts">

import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()
</script>

<template>
  <nav>
    <RouterLink class="navbar_link" to="/">EduSchedu</RouterLink>
    <div class="navbar_link">
      <p v-if="authStore.isLoggedIn">{{ authStore.user?.email }}</p>
      <RouterLink class="navbar_link" to="/login">Login</RouterLink>
<!--      <RouterLink class="navbar_link" to="/register">Register</RouterLink>-->
    </div>
  </nav>
</template>

<style src="src/assets/styles/Navbar.css">

</style>
