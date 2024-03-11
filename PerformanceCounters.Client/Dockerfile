FROM node:18-alpine

WORKDIR /react-docker/

COPY package.json package-lock.json /react-docker/
RUN npm install
EXPOSE 3000
COPY public/ /react-docker/public
COPY src/ /react-docker/src
ENV REACT_APP_HUB_ADDRESS=http://localhost:5068/hub/client
CMD ["npm", "start"]
