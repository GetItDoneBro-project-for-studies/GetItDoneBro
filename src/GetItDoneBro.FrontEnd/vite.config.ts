import { fileURLToPath, URL } from "node:url";
import fs from "fs";
import path from "path";

import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import { env } from "process";

const target =
  env["services__getitdonebro-api__https__0"] ??
  env["services__getitdonebro-api__http__0"] ??
  "https://localhost:7255";

// Generate or read SSL certificates for HTTPS
function getHttpsConfig() {
  const certDir = path.join(process.cwd(), ".certs");
  const certFile = path.join(certDir, "cert.pem");
  const keyFile = path.join(certDir, "key.pem");

  // Check if certificates already exist
  if (fs.existsSync(certFile) && fs.existsSync(keyFile)) {
    return {
      cert: fs.readFileSync(certFile),
      key: fs.readFileSync(keyFile),
    };
  }

  // If certificates don't exist, create them with a helpful message
  console.warn(
    "⚠️  SSL certificates not found. Please run: npm run setup-https"
  );
  return undefined;
}

const httpsConfig = getHttpsConfig();

export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    ...(httpsConfig && { https: httpsConfig }),
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
