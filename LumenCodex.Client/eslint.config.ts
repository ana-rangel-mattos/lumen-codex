import eslint from "@eslint/js";
import tseslint from "typescript-eslint";

export default tseslint.config(
  // Global ignores (replaces .eslintignore)
  {
    ignores: ["dist/**", "node_modules/**", "build/**"],
  },

  // Base configuration for JavaScript and TypeScript
  eslint.configs.recommended,
  ...tseslint.configs.recommended,

  // Custom project rules overrides
  {
    files: ["**/*.ts", "**/*.tsx"],
    rules: {
      "@typescript-eslint/no-unused-vars": [
        "warn",
        { argsIgnorePattern: "^_" },
      ],
      "@typescript-eslint/no-explicit-any": "warn",
      "no-console": ["warn", { allow: ["warn", "error"] }],
    },
  },
);
