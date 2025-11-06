function lazyLoad(view) {
  return () => import(`../components/${view}.vue`);
}

export const routes = [
  {
    name: "home",
    path: "/",
    component: lazyLoad("home-page"),
    display: "Forside",
    icon: "home",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Design din egen reol her",
      metaTags: [
        {
          name: "description",
          content:
            "Specialbyggede reoler, som du selv kan designe online. Et nemt og billigt alternativ til snedkerbyggede reoler og indbygningsreoler."
        },
        {
          property: "og:description",
          content:
            "Specialbyggede reoler, som du selv kan designe online. Et nemt og billigt alternativ til snedkerbyggede reoler og indbygningsreoler."
        }
      ]
    }
  },
  {
    name: "terms-and-conditions",
    path: "/handelsbetingelser-og-persondatapolitik",
    component: lazyLoad("terms-and-conditions"),
    display: "Handelsbetingelser og persondatapolitik",
    icon: "home",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Handelsbetingelser og persondatapolitik",
      metaTags: []
    }
  },
  {
    name: "shopping-cart",
    path: "/indkoebskurv",
    component: lazyLoad("checkout/stepwise-checkout-page"),
    display: "Indkøbskurv",
    icon: "shopping-basket",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Indkøbskurv"
    }
  },
  {
    name: "bookcase-designer",
    path: "/reoldesigner/:baseDesignChoice",
    component: lazyLoad("bookcase-designer-page"),
    display: "Design din reol",
    icon: "pen",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    props: true,
    meta: {
      title: "Shelfer - Reoldesigner",
      metaTags: [
        {
          name: "description",
          content:
            "Her kan du designe din egen reol. Vi producerer, leverer og monterer din egen helt unikke reol."
        },
        {
          property: "og:description",
          content:
            "Her kan du designe din egen reol. Vi producerer, leverer og monterer din egen helt unikke reol."
        }
      ]
    }
  },
  {
    name: "bookcase-designer",
    path: "/reoldesigner",
    component: lazyLoad("bookcase-designer-page"),
    display: "Design din reol",
    icon: "pen",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    props: true,
    meta: {
      title: "Shelfer - Reoldesigner",
      metaTags: [
        {
          name: "description",
          content:
            "Her kan du designe din egen reol. Vi producerer, leverer og monterer din egen helt unikke reol."
        },
        {
          property: "og:description",
          content:
            "Her kan du designe din egen reol. Vi producerer, leverer og monterer din egen helt unikke reol."
        }
      ]
    }
  },
  {
    name: "products",
    path: "/produkter",
    component: lazyLoad("products"),
    display: "Oplev produktet",
    icon: "home",
    type: "main-nav",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Produkter",
      metaTags: []
    }
  },
  {
    name: "standard-models",
    path: "/standardmodeller",
    component: lazyLoad("standard-models"),
    display: "Standardmodeller",
    icon: "home",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Standardmodeller",
      metaTags: []
    }
  },
  {
    name: "standard-model",
    path: "/standardmodeller/:modelId",
    component: lazyLoad("standard-model"),
    display: "Standardmodel",
    icon: "home",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    props: true,
    meta: {
      title: "Shelfer - Standardmodel",
      metaTags: []
    }
  },
  {
    name: "specifications-page",
    path: "/specifikationer",
    component: lazyLoad("specifications-page"),
    display: "Hent specifikationer",
    icon: "list",
    type: "user-menu",
    showWhenLoggedIn: true,
    showWhenLoggedOut: false,
    adminOnly: true,
    meta: {
      title: "Shelfer - Hent specifikationer"
    }
  },
  {
    name: "data-page",
    path: "/data",
    component: lazyLoad("data-page"),
    display: "Hent data",
    icon: "list",
    type: "user-menu",
    showWhenLoggedIn: true,
    showWhenLoggedOut: false,
    adminOnly: true,
    meta: {
      title: "Shelfer - Hent data"
    }
  },
  {
    name: "my-bookcases",
    path: "/mine-reoler",
    component: lazyLoad("my-shelfer/my-bookcases"),
    display: "Mine reoler",
    icon: "border-all",
    type: "user-menu",
    showWhenLoggedIn: true,
    showWhenLoggedOut: false,
    adminOnly: false,
    meta: {
      title: "Shelfer - Mine reoler"
    }
  },
  {
    name: "orders",
    path: "/ordrer",
    component: lazyLoad("my-shelfer/orders"),
    display: "Ordrer",
    icon: "truck",
    type: "user-menu",
    showWhenLoggedIn: true,
    showWhenLoggedOut: false,
    adminOnly: false,
    meta: {
      title: "Shelfer - Ordrer"
    }
  },
  {
    name: "contact-us",
    path: "/kontakt-os",
    component: lazyLoad("contact-us"),
    display: "Kontakt os",
    icon: "id-card",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Kontakt os",
      metaTags: [
        {
          name: "description",
          content:
            "Her finder du Shelfers kontaktoplysnigner. Kontakt os endelig, hvis du har spørgsmål, er i tvivl om noget eller har særlige ønsker til din reol."
        },
        {
          property: "og:description",
          content:
            "Her finder du Shelfers kontaktoplysnigner. Kontakt os endelig, hvis du har spørgsmål, er i tvivl om noget eller har særlige ønsker til din reol."
        }
      ]
    }
  },
  {
    name: "about-us",
    path: "/moed-shelfer",
    component: lazyLoad("about-us"),
    display: "Mød Shelfer",
    icon: "",
    type: "main-nav",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Mød Shelfer",
      metaTags: [
        {
          name: "description",
          content: "Her kan du læse om os, vores baggrund og vores visioner."
        },
        {
          property: "og:description",
          content: "Her kan du læse om os, vores baggrund og vores visioner."
        }
      ]
    }
  },
  {
    name: "frequently-asked-questions",
    path: "/ofte-stillede-sporgsmaal",
    component: lazyLoad("frequently-asked-questions"),
    display: "Ofte stillede spørgsmål",
    icon: "",
    type: "hidden",
    showWhenLoggedIn: true,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Ofte stillede spørgsmål",
      metaTags: [
        {
          name: "description",
          content: "Få svar på de mest almindelige spørgsmål."
        },
        {
          property: "og:description",
          content: "Få svar på de mest almindelige spørgsmål."
        }
      ]
    }
  },
  {
    name: "login-page",
    path: "/log-ind",
    component: lazyLoad("authentication/login-page"),
    display: "Log ind/opret konto",
    icon: "sign-in-alt",
    type: "user-menu",
    showWhenLoggedIn: false,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Log ind"
    }
  },
  {
    name: "register-page",
    path: "/opret-konto",
    component: lazyLoad("authentication/register-page"),
    display: "Opret konto",
    icon: "user-check",
    type: "hidden",
    showWhenLoggedIn: false,
    showWhenLoggedOut: true,
    adminOnly: false,
    meta: {
      title: "Shelfer - Opret konto"
    }
  },
  {
    name: "order-placed-page",
    path: "/ordre-bekraeftet",
    component: lazyLoad("checkout/order-placed-page"),
    display: "Ordre bekræftet",
    icon: "",
    type: "hidden",
    showWhenLoggedIn: false,
    showWhenLoggedOut: false,
    adminOnly: false,
    meta: {
      title: "Shelfer - Ordre oprettet"
    }
  },
  {
    name: "order-confirmation",
    path: "/ordrebekraeftelse",
    component: lazyLoad("checkout/order-confirmation"),
    display: "Ordrebekræftelse",
    icon: "",
    type: "hidden",
    showWhenLoggedIn: false,
    showWhenLoggedOut: false,
    adminOnly: false,
    props: true,
    meta: {
      title: "Shelfer - Ordrebekræftelse"
    }
  },
  {
    name: "forgot-password",
    path: "/glemt-adgangskode",
    component: lazyLoad("authentication/forgot-password"),
    display: "Glemt adgangskode",
    icon: "",
    type: "hidden",
    showWhenLoggedIn: false,
    showWhenLoggedOut: false,
    adminOnly: false,
    meta: {
      title: "Shelfer - Glemt adgangskode"
    }
  },
  {
    name: "password-change",
    path: "/skift-adgangskode/:recoveryKey",
    component: lazyLoad("authentication/password-change"),
    display: "Skift adgangskode",
    icon: "",
    type: "hidden",
    showWhenLoggedIn: false,
    showWhenLoggedOut: false,
    adminOnly: false,
    props: true,
    meta: {
      title: "Shelfer - Skift adgangskode"
    }
  },
  {
    name: "cookie-policy",
    path: "/cookiepolitik",
    component: lazyLoad("cookie-policy"),
    display: "Cookies og tracking",
    icon: "",
    type: "hidden",
    showWhenLoggedIn: false,
    showWhenLoggedOut: false,
    adminOnly: false,
    meta: {
      title: "Shelfer - Cookies og tracking"
    }
  }
  // {
  //   path: "/blog/",
  //   name: "blog-home",
  //   component: lazyLoad("blog/blog-home"),
  //   display: "Blog",
  //   icon: "",
  //   type: "main-nav",
  //   showWhenLoggedIn: true,
  //   showWhenLoggedOut: true,
  //   adminOnly: false,
  //   meta: {
  //     title: "Shelfer - Blog",
  //     metaTags: [
  //       {
  //         name: "description",
  //         content:
  //           "Her kan du læse om vores tanker, ideer, inspirationer og visioner."
  //       },
  //       {
  //         property: "og:description",
  //         content:
  //           "Her kan du læse om vores tanker, ideer, inspirationer og visioner."
  //       }
  //     ]
  //   }
  // },
  // {
  //   path: "/:slug",
  //   name: "blog-post",
  //   component: lazyLoad("blog/blog-post")
  // }
];
