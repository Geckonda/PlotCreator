export interface AuthUser {
  id: number
  login: string
  nickname: string
  email: string
  role: string
}

export interface LoginRequest {
  login: string
  password: string
}

export interface RegisterRequest {
  nickname: string
  login: string
  email: string
  password: string
  passwordConfirm: string
}
