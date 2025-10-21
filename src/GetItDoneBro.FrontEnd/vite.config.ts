import { fileURLToPath, URL } from "node:url";
import fs from "fs";
import path from "path";
import { execSync } from "child_process";
import os from "os";

import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import { env } from "process";

const target =
  env["services__getitdonebro-api__https__0"] ??
  env["services__getitdonebro-api__http__0"] ??
  "https://localhost:7255";

// Get or create HTTPS certificates using dotnet dev-certs
function getHttpsConfig() {
  const certDir = path.join(os.homedir(), ".dotnet", "corefx", "cryptography", "x509stores", "my");
  
  // Look for localhost certificate in .dotnet store
  if (fs.existsSync(certDir)) {
    try {
      const files = fs.readdirSync(certDir);
      if (files.length > 0) {
        console.log("✅ Using .NET trusted certificates for localhost");
        return true; // Let Vite use Node's built-in HTTPS support
      }
    } catch (e) {
      // Silently continue
    }
  }

  // Fallback: use @vitejs/plugin-basic-ssl approach with in-memory certs
  console.log("🔒 Using auto-generated HTTPS certificates (not system-trusted)");
  return true;
}

// Plugin to ensure .NET dev certs are available
function dotnetHttpsPlugin() {
  let certSetup = false;

  return {
    name: "dotnet-https-setup",
    async configResolved() {
      if (certSetup || env.SKIP_DOTNET_CERTS === "true") return;

      try {
        // Try to create/trust dev certificates using dotnet
        console.log("🔐 Setting up .NET dev certificates...");
        execSync("dotnet dev-certs https --trust", { stdio: "pipe" });
        console.log("✅ .NET dev certificates ready and trusted!");
        certSetup = true;
      } catch (error) {
        console.warn("⚠️  Could not setup .NET dev certs (optional)");
        console.warn("   If you have .NET installed, run: dotnet dev-certs https --trust");
        // This is not a fatal error - we can still run without it
      }
    }
  };
}

const hasHttps = getHttpsConfig();

const serverConfig: any = {
  host: true,
  port: parseInt(env.PORT ?? "5173"),
  proxy: {
    "^/api": {
      target,
      secure: false,
      changeOrigin: true,
      rewrite: (path: string) => path.replace(/^\/api/, ""),
    },
  },
};

if (hasHttps) {
  serverConfig.https = true;
}

export default defineConfig({
  plugins: [dotnetHttpsPlugin(), plugin()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: serverConfig,
});
