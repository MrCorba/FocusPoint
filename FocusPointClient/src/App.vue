<script setup lang="ts">
import { RouterView } from 'vue-router'
import NavBar from './components/NavBar.vue';
</script>

<template>
  <div class="app-container">
    <aside class="sidebar">
      <NavBar />
    </aside>
    <main class="content">
      <RouterView />
    </main>
  </div>
</template>

<style scoped lang="scss">
  @use 'styles/_variables.scss' as *;

.app-container {
  display: flex;
  height: 100vh;
}

.sidebar {
  width: $sidebar-width;
  background: $sidebar-color-background;
  color: $sidebar-color-text;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 14px 8px;
  gap: 8px;
  position: fixed;
  left: 0;
  top: 0;
  bottom: 0;
}

.content {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
  margin-left: $sidebar-width;
}
</style>