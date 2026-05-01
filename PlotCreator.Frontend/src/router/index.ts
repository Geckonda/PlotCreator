import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
      meta: { public: true },
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/RegisterView.vue'),
      meta: { public: true },
    },
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
    {
      path: '/settings/types',
      name: 'type-manager',
      component: () => import('@/views/TypeManagerView.vue'),
    },
    {
      path: '/world/:id/graphs/:graphId?',
      name: 'graph-view',
      component: () => import('@/views/GraphView.vue'),
    },
  ],
})

router.beforeEach(async (to) => {
  const auth = useAuthStore()
  if (!auth.ready) {
    await auth.fetchMe()
  }
  const isPublic = to.matched.some((r) => r.meta.public)
  if (!auth.isAuthenticated && !isPublic) {
    return {
      name: 'login',
      query: to.fullPath !== '/' ? { redirect: to.fullPath } : undefined,
    }
  }
  if (auth.isAuthenticated && isPublic) {
    return { name: 'home' }
  }
  return true
})

export default router
