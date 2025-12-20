<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <div class="app-container">
    <nav class="sidebar">
      <h2>FocusPoint</h2>
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

<style scoped>
.app-container {
  display: flex;
  height: 100vh;
}

.sidebar {
  width: 250px;
  background-color: #f5f5f5;
  padding: 20px;
  border-right: 1px solid #ddd;
}

.sidebar h2 {
  margin: 0 0 20px 0;
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
  background-color: #e0e0e0;
}

.sidebar a.router-link-active {
  background-color: #007bff;
  color: white;
}

.content {
  flex: 1;
  padding: 20px;
  overflow-y: auto;
}
</style>