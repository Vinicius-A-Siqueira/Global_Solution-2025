// scripts/generate-commit.js
const { execSync } = require('child_process');
const fs = require('fs');
try {
  const hash = execSync('git rev-parse --short HEAD').toString().trim();
  const content = `export const COMMIT_HASH = "${hash}";\n`;
  if (!fs.existsSync('src/config')) fs.mkdirSync('src/config', { recursive: true });
  fs.writeFileSync('src/config/commit.js', content);
  console.log('src/config/commit.js gerado:', hash);
} catch (e) {
  console.error('Erro ao gerar commit hash. Certifique-se de estar em um repositório git.', e.message);
  process.exit(1);
}
