export class TestTemplate {
    name!: string;
    description!: string
    subject!: string;
    content!: string;
    isHtml!: boolean;
    mailId!: number;
    secret!: string;
    recipient!: {
        email: string,
        args: {[key:string]: string}
    };
}