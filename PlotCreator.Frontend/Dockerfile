# ── Stage 1: build the SPA ──────────────────────────────────────────────────
FROM node:20-alpine AS build
WORKDIR /app

# Cache deps based on lockfile
COPY package.json package-lock.json* ./
RUN npm ci

# Build (axios baseURL is /api so the SPA talks to /api on its own origin,
# which nginx proxies to the api container)
COPY . .
RUN npm run build

# ── Stage 2: serve with nginx ───────────────────────────────────────────────
FROM nginx:alpine AS runtime

# Replace the default site config with our SPA + /api proxy
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Static SPA bundle
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
