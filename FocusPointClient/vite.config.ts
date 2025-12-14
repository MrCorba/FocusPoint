import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
  plugins: [vue()],
 build: {
    outDir: '../wwwroot', // Output to .NET's static files folder
    emptyOutDir: true
  },
  server: {
    port: 5173,
    proxy: {
      '/api': 'https://localhost:5001' // Proxy API calls to .NET backend
    }
  }
})
