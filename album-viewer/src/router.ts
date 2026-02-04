import { createRouter, createWebHistory } from "vue-router";
import Layout from "./Layout.vue";
import AlbumsPage from "./pages/AlbumsPage.vue";
import Graph from "./Graph.vue";

const routes = [
  {
    path: "/",
    component: Layout,
    children: [
      {
        path: "",
        component: AlbumsPage,
        name: "Home",
      },
      {
        path: "graph",
        component: Graph,
        name: "Graph",
      },
    ],
  },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});
