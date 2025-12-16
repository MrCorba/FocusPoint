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
      '/api': {
        target: 'http://localhost:5082', // .NET backend
        changeOrigin: true,               // needed for virtual hosts
        secure: false                     // allow self-signed SSL in dev
      }
  }
}
})
