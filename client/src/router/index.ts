import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import CreateUserView from '@/views/CreateUserView.vue'
import CreateSchoolView from '@/views/CreateSchoolView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView
    },
    {
      path: '/schools',
      name: 'schools',
      component: SchoolsView
    },
    {
      path: '/school_create',
      name: 'school',
      component: CreateSchoolView
    },
    {
      path: '/:school_id/create_user',
      name: 'create_user',
      component: CreateUserView
    }
  ],
})

export default router
