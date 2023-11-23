const PROXY_CONFIG = [
  {
    context: [
      "/api/employees",
      "/api/roles",
      "/api/employees/managers",
    ],
    target: "https://localhost:7154",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
