FROM node:18.13.0 AS build-stage

WORKDIR /app

COPY package*.json /app/

RUN npm install

COPY . /app/

RUN npm run build

FROM nginx:alpine AS publish-stage

COPY --from=build-stage /app/dist/front-end/browser/ /usr/share/nginx/html

EXPOSE 80 

CMD ["nginx", "-g", "daemon off;"]