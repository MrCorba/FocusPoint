<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
    <nav class="nav-main">
        <RouterLink to="/">
            <font-awesome-icon :icon="['fas', 'home']" />
        </RouterLink>
    </nav>

    <nav class="nav-bottom">
        <RouterLink to="/settings">
            <font-awesome-icon :icon="['fas', 'cogs']" />
        </RouterLink>
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

a {
    font-size: 24px;
    color: $sidebar-color-text;
    border-radius: 4px;
    padding: 10px;

    &.router-link-active {
        background-color: $color-primary;
        color: $color-primary-text;
    }

    &:hover{
        background-color: $color-primary-hover;
        color: $color-primary-text;
    }
}
</style>