<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="app-container">
    <nav class="sidebar">
      <ul>
        <li><RouterLink to="/">Home</RouterLink></li>
        <li><RouterLink to="/settings">Settings</RouterLink></li>
      </ul>
    </nav>
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
  margin: 0;
  padding: 0;
}

.sidebar {
  width: 250px;
  background-color: $color-primary;
  color: $color-primary-text;
}


.sidebar ul {
  list-style: none;
  padding: 0;
}

.sidebar li {
  margin-bottom: 10px;
}

.sidebar a {
  text-decoration: none;
  color: #333;
  padding: 8px 12px;
  display: block;
  border-radius: 4px;
}

.sidebar a:hover {
  background-color: $color-primary-hover;
}

.sidebar a.router-link-active {
  background-color: $color-primary-active;
  color: $color-primary-text;
}

.content {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
}
</style>