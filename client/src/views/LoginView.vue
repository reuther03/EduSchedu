<script setup lang="ts">

import { reactive } from 'vue'
import router from '@/router'
import axiosService from '@/services/axiosService'
import type { ILoginResult } from '@/Results/ILoginResult'
import { useAuthStore } from '@/stores/authStore'
import type { UserIndentityModel } from '@/types/auth'

const authStore = useAuthStore()

const form = reactive({
  email: '',
  password: ''
})

const handleSubmit = async () => {
  const user = {
    email: form.email,
    password: form.password
  }
  try {
    const result = await axiosService.post<ILoginResult>('/users-module/Users/login', user)

    if (result.data.isSuccess) {
      const identity = result.data.value as UserIndentityModel
      authStore.authenticate(identity)
    } else {
      console.error('Error logging in', result.data.message)
      return new Error(result.data.message)
    }

    await router.push('/')
  } catch (e) {
    console.error('Error', e)
  }
}
</script>

<template>
  <form @submit.prevent="handleSubmit" class="login_card">
      <label for="email">Email
        <input v-model="form.email" type="email" id="email" name="email"  autocomplete="email"/>
      </label>
      <label for="password">Password
        <input v-model="form.password" type="password" id="password" name="password" autocomplete="current-password"/>
      </label>
      <button type="submit" class="button-1">Login</button>
  </form>
</template>

<style src="../assets/styles/Login.css">

</style>
