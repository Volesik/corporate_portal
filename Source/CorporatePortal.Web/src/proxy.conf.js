const PROXY_CONFIG = [
  {
    context: [
      "/userinfo",
    ],
    target: "http://localhost:5000",
    secure: false
  },
]

module.exports = PROXY_CONFIG;
