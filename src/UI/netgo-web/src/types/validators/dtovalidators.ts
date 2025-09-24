import { z } from "zod"

export const RegistrationRequestValidator = z.object({
  FirstName: z
    .string()
    .min(2, {
      message: "Username must be at least 2 characters.",
    })
    .max(30, {
      message: "Username must not be longer than 30 characters.",
    }),
  LastName: z
    .string({
      message: "Please enter your lastname.",
    }),
  Email: z
    .string({
        message: "Please enter your email."
    }),
  Password: z
    .string({
        message: "Please enter your password"
    }) 
})

export const AuthRequestValidator = z.object({
  Email: z
    .string({
        message: "Please enter your email."
    }),
  Password: z
    .string({
        message: "Please enter your password"
    }) 
})