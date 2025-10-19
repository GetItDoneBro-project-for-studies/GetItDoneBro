import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import { env } from "process";

const target =
  env["services__getitdonebro-api__https__0"] ??
  env["services__getitdonebro-api__http__0"] ??
  "https://localhost:7255";

export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    host: true,
    port: parseInt(env.PORT ?? "5173"),
    proxy: {
      "^/api": {
        target,
        secure: false,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ""),
      },
    },
  },
});
