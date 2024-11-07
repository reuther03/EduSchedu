import { defineStore } from 'pinia'
import type { UserIndentityModel } from '@/types/auth'
import tokenService from '@/services/tokenService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: null as string | null,
    user: null as UserIndentityModel | null
  }),

  getters: {
    isLoggedIn: (state) => !!state.token
  },

  actions: {
    authenticate(identity: UserIndentityModel) {
      this.user = identity
      const token = identity.token || tokenService.getToken()
      this.setToken(token || '')
    },

    setToken(token: string) {
      this.token = token
      tokenService.setToken(token)
    },

    logout() {
      this.token = null
      this.user = null
      tokenService.removeToken()
    }
  }
})
