<script setup lang="ts">

import { useAuthStore } from '@/stores/authStore'
import { reactive } from 'vue'
import axios from 'axios'
import type { BaseResult } from '@/Results/BaseResult'
import axiosService from '@/services/axiosService'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

const form = reactive({
  name: '',
  city: '',
  street: '',
  zipCode: '',
  mapCoordinates: '' as string | null,
  phoneNumber: '',
  email: '',
})

const handleSubmit = async () => {
  const school = {
    name: form.name,
    city: form.city,
    street: form.street,
    zipCode: form.zipCode,
    mapCoordinates: form.mapCoordinates,
    phoneNumber: form.phoneNumber,
    email: form.email,
  }
  try{
    const result = await axiosService.post<BaseResult>("schools-module/Schools/create", school)
    console.log(result)
  } catch (e) {
    console.error('Error', e)
  }
}
</script>

<template v-if="authStore.isLoggedIn">
  <form class="login_card" @submit.prevent="handleSubmit">
    <label for="name">Name*
      <input v-model="form.name" type="text" id="name" name="name" required />
    </label>
    <label for="city">City*
      <input v-model="form.city" type="text" id="city" name="city" required/>
    </label>
    <label for="street">Street*
      <input v-model="form.street" type="text" id="street" name="street" required/>
    </label>
    <label for="zipCode">Zip code*
      <input v-model="form.zipCode" type="text" id="zipCode" name="zipCode" required/>
    </label>
    <label for="mapCoordinates">Map coordinates
      <input v-model="form.mapCoordinates" type="text" id="mapCoordinates" name="mapCoordinates" placeholder="Optional" />
    </label>
    <label for="phoneNumber">Phone number*
      <input v-model="form.phoneNumber" type="text" id="phoneNumber" name="phoneNumber" required/>
    </label>
    <label for="email">Email*
      <input v-model="form.email" type="email" id="email" name="email" required/>
    </label>
    <button class="button-1" type="submit">Create school</button>
  </form>
</template>

<style scoped>

</style>
