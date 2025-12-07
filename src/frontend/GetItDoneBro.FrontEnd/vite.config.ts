import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "vite";
import plugin from "@vitejs/plugin-react";
import { env } from "process";
import { spawnSync } from "child_process";
import fs from "fs";
import path from "path";
import os from "os";

const target =
  env["services__getitdonebro-api__https__0"] ??
  env["services__getitdonebro-api__http__0"] ??
  "https://localhost:7255";

// Function to get HTTPS certificates from .NET dev-certs
function getHttpsCertificates(): { cert: string; key: string } | null {
  try {
    const certDir = path.join(os.homedir(), ".dotnet", "https");
    
    // Ensure directory exists
    if (!fs.existsSync(certDir)) {
      fs.mkdirSync(certDir, { recursive: true });
    }

    const certPath = path.join(certDir, "localhost.crt");
    const keyPath = path.join(certDir, "localhost.key");

    // Export certificates if they don't exist
    if (!fs.existsSync(certPath) || !fs.existsSync(keyPath)) {
      console.log("🔐 Exporting .NET dev certificates...");
      
      // Export to PEM using dotnet CLI
      const result = spawnSync("dotnet", [
        "dev-certs",
        "https",
        "--export-path",
        certPath,
        "--format",
        "Pem",
        "--no-password"
      ], {
        encoding: "utf-8",
        stdio: "pipe"
      });

      if (result.status !== 0) {
        console.warn("⚠️ Could not export certificates:", result.stderr);
        return null;
      }

      // Key is usually in the same file for PEM format
      if (fs.existsSync(certPath)) {
        const pemContent = fs.readFileSync(certPath, "utf-8");
        
        // Extract certificate and key from PEM file
        const certMatch = pemContent.match(/(-----BEGIN CERTIFICATE-----[\s\S]*?-----END CERTIFICATE-----)/);
        const keyMatch = pemContent.match(/(-----BEGIN PRIVATE KEY-----[\s\S]*?-----END PRIVATE KEY-----)/);
        
        if (certMatch && keyMatch) {
          fs.writeFileSync(certPath, certMatch[1]);
          fs.writeFileSync(keyPath, keyMatch[1]);
          console.log("✅ .NET dev certificates ready!");
          return {
            cert: certMatch[1],
            key: keyMatch[1]
          };
        }
      }
    }

    // Try to read existing certificates
    if (fs.existsSync(certPath) && fs.existsSync(keyPath)) {
      console.log("✅ Using .NET dev certificates");
      return {
        cert: fs.readFileSync(certPath, "utf-8"),
        key: fs.readFileSync(keyPath, "utf-8")
      };
    }
  } catch (error) {
    console.warn("⚠️ Error loading HTTPS certificates:", error);
  }

  return null;
}

const httpsConfig = getHttpsCertificates();

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

if (httpsConfig) {
  serverConfig.https = httpsConfig;
}

export default defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: serverConfig,
});
