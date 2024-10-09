const PROXY_CONFIG = [
  {
    context: [
      "/userinfo",
    ],
    target: "http://localhost:5116",
    secure: false
  },
]

module.exports = PROXY_CONFIG;
