
import { ArrowRight, Search } from "lucide-react"
import { Button } from "@/components/ui/button"
import {
  Drawer,
  DrawerContent,
  DrawerHeader,
  DrawerTrigger,
  DrawerTitle
}  from "./ui/drawer"

import { Input } from "@/components/ui/input"

import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"

import { zodResolver } from "@hookform/resolvers/zod"
import { z } from "zod"
import { useForm } from "react-hook-form"
import { AuthRequestValidator, RegistrationRequestValidator } from "@/types/validators/dtovalidators"
import { useNavigate } from "react-router-dom"
import { login } from "@/Services/authService"
import type { AuthRequest } from "@/types/dtos"

type RegisterFormValues = z.infer<typeof RegistrationRequestValidator>
type LoginFormValues = z.infer<typeof AuthRequestValidator>


export default function Navbar() {
  const registerform = useForm<RegisterFormValues>({
    resolver: zodResolver(RegistrationRequestValidator),
    mode: "onChange",
  })

  const loginform = useForm<LoginFormValues>({
    resolver: zodResolver(AuthRequestValidator),
    mode: "onChange",
  })
  

  function onLoginSubmit() {
    const data = loginform.getValues();
    const loginRequest: AuthRequest = {
      email: data.Email,
      password: data.Password
    };
    login(loginRequest);
  }

  function onRegisterSubmit() {
    console.log("asdf")
  }
  
  const navigate = useNavigate();

  const goToHome = () => {
    navigate("/");
  };

  return (
    <header className="border-b px-4 md:px-6">
      <div className="flex h-16 items-center justify-between gap-4">
        <div className="flex items-center gap-2">
          <div className="flex items-center gap-6">
            <button className="cursor-pointer" onClick={goToHome}><h1>Netgo</h1></button>
          </div>
        </div>
        <div className="relative flex h-10 w-full max-w-100" role="group">
          <Input type="text" placeholder="Search..." />
          <Button className="!absolute right-0 top-0 z-10" type="submit">
            <Search />
          </Button>
        </div>
        <div className="flex items-center gap-2">
          <Drawer>
            <DrawerTrigger><Button variant={"outline"} size={"sm"}>Sign In</Button></DrawerTrigger>
            <DrawerContent>
              <DrawerHeader>
                <DrawerTitle className="text-xl mb-5">Sign In</DrawerTitle>
                <div className="flex justify-center w-full">
                  <Form {...loginform}>
                    <form onSubmit={loginform.handleSubmit(onLoginSubmit)} className="w-2/3 space-y-6 max-w-100">
                      <div className="grid flex grid-cols-1 gap-5">
                          <FormField
                            control={loginform.control}
                            name="Email"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Email</FormLabel>
                                <FormControl>
                                  <Input type="email" placeholder="Your email address" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />     
                          <FormField
                            control={loginform.control}
                            name="Password"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Last Name</FormLabel>
                                <FormControl>
                                  <Input placeholder="Your password" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />                  
                      </div>
                      <Button type="submit"><ArrowRight size={25} /> Continue</Button>
                    </form>
                  </Form>    
                </div>              
              </DrawerHeader>
            </DrawerContent>
          </Drawer>
          <Drawer>
            <DrawerTrigger><Button size={"sm"}>Get Started</Button></DrawerTrigger>
            <DrawerContent>
              <DrawerHeader>
                <DrawerTitle className="text-xl mb-5">Register</DrawerTitle>
                <div className="flex justify-center w-full">
                  <Form {...registerform}>
                    <form onSubmit={registerform.handleSubmit(onRegisterSubmit)} className="w-2/3 space-y-6">
                      <div className="grid flex grid-cols-1 md:grid-cols-2 gap-5">
                          <FormField
                            control={registerform.control}
                            name="FirstName"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>First Name</FormLabel>
                                <FormControl>
                                    <Input placeholder="Your first name" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />
                          <FormField
                            control={registerform.control}
                            name="LastName"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Last Name</FormLabel>
                                <FormControl>
                                  <Input placeholder="Your last name" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />      
                          <FormField
                            control={registerform.control}
                            name="Email"
                            render={({ field }) => (
                              <FormItem>
                                <FormLabel>Email</FormLabel>
                                <FormControl>
                                  <Input type="email" placeholder="Your email address" {...field} />
                                </FormControl>
                                <FormMessage />
                              </FormItem>
                            )}
                          />                      
                      </div>
                      <Button type="submit"><ArrowRight size={25} /> Continue</Button>
                    </form>
                  </Form>    
                </div>              
              </DrawerHeader>
            </DrawerContent>
          </Drawer>
        </div>
      </div>
    </header>
  )
}
