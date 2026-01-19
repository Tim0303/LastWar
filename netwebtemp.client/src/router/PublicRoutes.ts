const PublicRoutes = {
  path: '/',
  // component: () => import('@/layouts/blank/BlankLayout.vue'),
  component: () => import('@/layouts/full/FullLayout.vue'),
  meta: {
    requiresAuth: false
  },
  children: [
    // {
    //   name: 'Authentication',
    //   path: '/login',
    //   component: () => import('@/views/authentication/LoginPage.vue')
    // },
    // {
    //   name: 'Login',
    //   path: '/login1',
    //   component: () => import('@/views/authentication/auth/LoginPage.vue')
    // },
    // {
    //   name: 'Register',
    //   path: '/register',
    //   component: () => import('@/views/authentication/auth/RegisterPage.vue')
    // },
    {
      name: 'Error 404',
      path: '/error',
      component: () => import('@/views/pages/maintenance/error/Error404Page.vue')
    },

    {
      name: 'Player Power List',
      path: '/info/playerpowerlist',
      component: () => import('@/views/info/PlayerPowerList.vue')
    },
    {
      name: 'UpdatePlayerPower',
      path: '/info/updateplayerpower',
      component: () => import('@/views/info/UpdatePlayerPower.vue')
    }
  ]
};

export default PublicRoutes;
