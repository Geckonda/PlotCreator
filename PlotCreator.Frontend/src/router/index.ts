import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/world/:id',
      name: 'world',
      component: () => import('@/views/WorldView.vue'),
    },
    {
      path: '/world/:id/entity/:entityId',
      name: 'entity-detail',
      component: () => import('@/views/EntityDetailView.vue'),
      props: (route) => ({
        worldId: Number(route.params.id),
        entityId: Number(route.params.entityId),
      }),
    },
  ],
})

export default router
