<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
    <nav class="nav-main">
        <RouterLink to="/">Home</RouterLink>
    </nav>

    <nav class="nav-bottom">
        <RouterLink to="/settings">Settings</RouterLink>
    </nav>
</template>

<style scoped lang="scss">
@use '../styles/_variables.scss' as *;

.nav-main {
    flex: 1 1 auto;
    display: flex;
    flex-direction: column;
    justify-content: center;
    gap: 12px;
    ;
    align-items: center;
    width: 100%;
}

.nav-bottom {
    align-self: center;
    margin-bottom: 4px;
}


a.router-link-active {
    background-color: $color-primary;
    color: $color-primary-text;
}
</style>