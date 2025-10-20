#!/usr/bin/env node

import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";
import { execSync } from "child_process";
import { platform } from "os";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const rootDir = path.join(__dirname, "..");
const certDir = path.join(rootDir, ".certs");
const certFile = path.join(certDir, "cert.pem");
const keyFile = path.join(certDir, "key.pem");

// Create .certs directory if it doesn't exist
if (!fs.existsSync(certDir)) {
  fs.mkdirSync(certDir, { recursive: true });
  console.log(`✓ Created .certs directory`);
}

// Check if certificates already exist
if (fs.existsSync(certFile) && fs.existsSync(keyFile)) {
  console.log("✓ SSL certificates already exist");
  process.exit(0);
}

console.log("Generating self-signed SSL certificates...");

try {
  // Use OpenSSL to generate self-signed certificate
  const isWindows = platform() === "win32";
  const subj = isWindows
    ? "/CN=localhost"
    : "/C=US/ST=State/L=City/O=Organization/CN=localhost";

  const opensslCmd = `openssl req -x509 -newkey rsa:2048 -keyout "${keyFile}" -out "${certFile}" -days 365 -nodes -subj "${subj}"`;

  execSync(opensslCmd, { stdio: "inherit" });
  console.log("✓ SSL certificates generated successfully!");
  console.log(`  Cert: ${certFile}`);
  console.log(`  Key:  ${keyFile}`);
} catch (error) {
  console.error("Error generating certificates:", error.message);
  console.error(
    "\nFailed to generate certificates. Please ensure OpenSSL is installed on your system."
  );
  console.error("\nTo install OpenSSL:");
  console.error("  Windows: choco install openssl OR winget install OpenSSL");
  console.error("  macOS: brew install openssl");
  console.error("  Linux: sudo apt-get install openssl");
  process.exit(1);
}
