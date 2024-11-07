const tokenService = {
  getToken() {
    return localStorage.getItem('token')
},
  setToken(token : any) {
    localStorage.setItem('token', token)
  },
  removeToken(){
    localStorage.removeItem('token');
  },
}

export default tokenService;
