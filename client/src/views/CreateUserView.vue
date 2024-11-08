<script setup lang="ts">

import { useRoute } from 'vue-router'
import { reactive } from 'vue'
import { UserRole } from '@/types/enums'

const route = useRoute()

const schoolId = route.params.schoolId

const form = reactive({
  email: '',
  fullName: '',
  role: UserRole,
})

const handleSubmit = async () => {

}

</script>

<template>
  <div>
    <h1>Create User</h1>
  </div>
</template>

<style scoped>

</style>
